import {Button, TextField} from '@material-ui/core';
import React, {Fragment, SyntheticEvent, useEffect, useState} from 'react';
import {createGame} from "../../redux/games/actions";
import {StylesProvider} from '@material-ui/core/styles';
import s from './GamePage.module.scss';
import GameLocalHistory from '../GameLocalHistory/GameLocalHistory';
import {TimelineMax} from 'gsap';
import HeaderComponent from '../HeaderComponent/Header';
import {selectorGetSettings} from '../../redux/settings/selector';
import {Store} from '../../redux/root';
import {connect} from 'react-redux';
import {IDifficult} from '../../redux/settings/interfaces';
import { GameResultType } from '../../core/_consts/enums';
import { selectorGetUser } from '../../redux/user/selector';
import { VaultService } from '../../utils/storage';
import { IUser } from '../../redux/user/interfaces';

function getRandomArbitrary(min: number, max: number) {
  return Math.floor(Math.random() * (max - min) + min);
}
const GuessResultType = Object.freeze({lower: 1, bigger: 2});

export interface IGuess {
  guessNumber: number;
  guessResultType: number;
  isWin: boolean;
}

const getNumberToGuess = (numberFrom: number, numberTo: number) => {
  return getRandomArbitrary(getRandomArbitrary(numberFrom, numberTo / 10), getRandomArbitrary(numberTo / 10, numberTo));
};

const GamePage: React.FC = ({settings, createGame}: any) => {
  const [inputNumber, setInputNumber] = useState(0);
  const [guessingHistory, setGuessHistory] = useState<Array<IGuess>>([]);
  const [currentGameRetriesRemained, setCurrentGameRetriesRemained] = useState(settings.difficult.retriesCount);
  const [showResultComponent, setShowResultComponent] = useState({display: false, type: 'Lose'});
  const [isTryButtonDisabled, setIsTryButtonDisabled] = useState(false);
  const [randomNumber, setRandomNumber] = useState(
    getNumberToGuess(settings.difficult.numberFrom, settings.difficult.numberTo)
  );
  const [hints, setHints] = useState({min: settings.difficult.numberFrom, max: settings.difficult.numberTo});

  const pewOnClick = (isLeft: boolean) => {
    let timeLine = new TimelineMax({
      paused: false,
    });
    let button = document.querySelector('.pew');

    timeLine
      .to(button, 0.4, {
        display: 'block',
        width: 0.2,
        opacity: 0.5,
        boxShadow: '0px 0px 35px 7px #cd28fa',
        delay: 0.25,
      })
      .to(button, 0.1, {
        opacity: 0.5,
        background: '#26ff92',
      })
      .to(button, 0, {
        height: 1,
        delay: 0.2,
      })
      .to(button, 0.1, {
        boxShadow: '0px 0px 100px 55px #fa2856',
        x: !isLeft ? 90 : -90,
        width: 100,
        delay: 0.23,
      })
      .to(button, 0.3, {
        width: 800,
        x: !isLeft ? -2000 : 2000,
        boxShadow: '0px 0px 85px 17px #fa2856',
        opacity: 0,
        delay: 0.1,
      })
      .to(button, 0, {
        opacity: 0,
        display: 'none',
        x: 0,
        boxShadow: 0,
        width: '100%',
        height: '100%',
      });
  };

  const tryButtonOnClick = () => {
    console.log(randomNumber);
    console.log(inputNumber);
    let historyElement = {
      guessNumber: inputNumber,
      guessResultType: inputNumber < randomNumber ? GuessResultType.bigger : GuessResultType.lower,
      isWin: inputNumber == randomNumber,
    };
    setIsTryButtonDisabled(true);
    pewOnClick(historyElement.guessResultType === 1 ? true : false);
    setGuessHistory((prevState) => {
      return [...prevState, historyElement];
    });
    setCurrentGameRetriesRemained(currentGameRetriesRemained - 1);
    setHints((prevState) => {
      return {
        min:
          historyElement.guessResultType === GuessResultType.bigger && prevState.min < inputNumber - -1
            ? inputNumber - -1
            : prevState.min,
        max:
          historyElement.guessResultType === GuessResultType.lower && prevState.max > inputNumber - 1
            ? inputNumber - 1
            : prevState.max,
      };
    });
  };

  const store = new VaultService();
  const user:IUser = store.getItem("currentUser");
  

  useEffect(() => {
    if (!!guessingHistory.length && guessingHistory[guessingHistory.length - 1].guessNumber == randomNumber) {
      setShowResultComponent((prevState) => {
        return {...prevState, display: true, type: 'Victory'};
      });
      createGame({
        id:0, gameResultType: GameResultType.Win, playerId: user.id
      });
    }
    if (currentGameRetriesRemained === 0 && guessingHistory[guessingHistory.length - 1].guessNumber != randomNumber) {
      setShowResultComponent((prevState) => {
        return {...prevState, display: true, type: 'Defeat'};
      });
      createGame({
        id:0, gameResultType: GameResultType.Lose, playerId: user.id
      });
    }

    const handleButtonPress = (event:any) => {
      if (!isTryButtonDisabled && !showResultComponent.display){
        switch (event.key) {
          case 'Enter':
            tryButtonOnClick();
            break;
          default:
            break;
        }
      }
    }
    document.addEventListener("keydown", handleButtonPress);
    return () => {document.removeEventListener("keydown", handleButtonPress)};
  }, [currentGameRetriesRemained, isTryButtonDisabled]);

  const guessInputOnChange = (event: any) => {
    setInputNumber(event.target.value);
    setIsTryButtonDisabled(false);
  };
  const startNewGame = (difficult: IDifficult) => {
    setInputNumber(0);
    setGuessHistory([]);
    setCurrentGameRetriesRemained(difficult.retriesCount);
    setRandomNumber(getNumberToGuess(settings.difficult.numberFrom, settings.difficult.numberTo));
    setShowResultComponent({display: false, type: 'Lose'});
    setHints({min: settings.difficult.numberFrom, max: settings.difficult.numberTo});
  };
  return (
    <StylesProvider injectFirst>
      <Fragment>
        <div className={s.gamePageRoot}>
          {!!guessingHistory.length && <GameLocalHistory guessingHistory={guessingHistory} />}
          {showResultComponent.display ? (
            <div className={s.gameResultContainer}>
              <div className={s.gameResult}>{showResultComponent.type}</div>
              <Button
                className={s.guessButton}
                variant="contained"
                color="primary"
                onClick={() => {
                  startNewGame(settings.difficult);
                }}
              >
                New game
              </Button>
            </div>
          ) : (
            <>
              <div style={{color: 'white', fontSize: '25px', margin: '20px'}}>
                Retries remained = {currentGameRetriesRemained}
              </div>
              <div className={s.inputContainer}>
                {settings.isHintModeEnabled && <div className={s.hint}>{hints.min}</div>}
                <TextField
                  className={s.guessInput}
                  id="outlined-basic"
                  label="GUESS"
                  variant="outlined"
                  type="number"
                  onChange={guessInputOnChange}
                />
                {settings.isHintModeEnabled && <div className={s.hint}>{hints.max}</div>}
              </div>
              <Button
                className={s.guessButton}
                variant="contained"
                color="primary"
                onClick={tryButtonOnClick}
                disabled={isTryButtonDisabled}
              >
                Try
                <div className={"pew"}></div>
              </Button>
            </>
          )}
        </div>
      </Fragment>
    </StylesProvider>
  );
};

const mapStateToProps = (state: Store) => ({
  settings: selectorGetSettings(state)
});
const mapDispatchToProps = {
  createGame
}
export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
