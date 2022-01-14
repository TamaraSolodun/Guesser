import {List, ListItem, ListItemText, ListSubheader} from '@material-ui/core';
import s from './GameLocalHistory.module.scss';
import {IGuess} from '../GamePage/GamePage';

const GameLocalHistory = ({guessingHistory}:{guessingHistory:Array<IGuess>}) => {
  return (
    <div className={s.listContainer}>
        <List
          component="div"
          aria-labelledby="nested-list-subheader"
          subheader={
            <ListSubheader component="div" id="nested-list-subheader">
              Current game guessing history
            </ListSubheader>
          }
          className={s.listRoot}
        >
          <div className={s.liContainer}>
            {guessingHistory.map((item, index) => {
              return (<ListItem>
                        <ListItemText key={index} primary={index + 1 + ". " + item.guessNumber} secondary={item.isWin ? "Congratulation!" : (item.guessResultType === 2 ? "Your number is too small!" : "Your number is too big!")} />
                      </ListItem>);
            })}
          </div>
          
        </List>
    </div>
  );
};

export default GameLocalHistory;
