import {List, ListItem, ListItemText, ListSubheader} from '@material-ui/core';
import s from './GameHistory.module.scss';
import { connect } from 'react-redux';
import { Store } from '../../redux/root';
import { VaultService } from '../../utils/storage';
import { IUser } from '../../redux/user/interfaces';
import { useEffect, useState } from 'react';
import { selectorGetGames } from '../../redux/games/selector';
import { getGames } from '../../redux/games/actions';
import { IGame } from '../../redux/games/interfaces';
import { GameResultType } from '../../core/_consts/enums';

const GameHistory = ({games, getGames, selectorGetGames}:any) => {
  const store = new VaultService();
  const user:IUser = store.getItem("currentUser");
  const [gamesLocal, setGames] = useState<Array<IGame>>(games);
  useEffect(() => {
    getGames({playerId: user.id});
  }, []);
  useEffect(() => {
    setGames(games);
  }, [games]);
  return (
    <div className={s.listContainer}>
        <List
          component="div"
          aria-labelledby="nested-list-subheader"
          subheader={
            <ListSubheader component="div" id="nested-list-subheader">
              Games history
            </ListSubheader>
          }
          className={s.listRoot}
        >
          <div className={s.liContainer}>
            {gamesLocal && gamesLocal.map((item:IGame, index:number) => {
              return (<ListItem key={index+item.gameResultType}>
                        <ListItemText key={index} primary={index + 1 + ". " + GameResultType[item.gameResultType]} secondary={"Played at: " + item.playedAt} />
                      </ListItem>);
            })}
          </div>
          
        </List>
    </div>
  );
};

const mapStateToProps = (state: Store) => ({
  games: selectorGetGames(state)
});
const mapDispatchToProps = {
  getGames,
  selectorGetGames
}

export default connect(mapStateToProps, mapDispatchToProps)(GameHistory);
