import React, { useEffect } from 'react';
import './index.css';
import HomePage from './components/HomePage/HomePage';
import MainLayout from './components/MainLayout/MainLayout';
import GamePage from './components/GamePage/GamePage';
import { Route, Switch, useHistory } from 'react-router-dom';
import { ROUTES } from './core/_consts/routes';
import WelcomePage from './components/WelcomePage/WelcomePage';
import ConfirmAccount from './components/ConfirmAccount/ConfirmAccount';
import { VaultService } from './utils/storage';
import AlertHandler from './components/AlertHandler';
import GameHistory from './components/GameHistory/GameHistory';

const App = () => {
  const history = useHistory();
  const storage = new VaultService();
  useEffect(() => {
    if(!storage.getItem("token") && history.location.pathname != ROUTES.WELCOME_SIGN_UP){
      history.push(ROUTES.WELCOME);
    }
  })
  return (
    <div id="app-root">
      <AlertHandler/>
      <Switch>
          <MainLayout>
              <Route exact path={ROUTES.WELCOME} render={() => <WelcomePage formTypeInitValue={"signIn"}/>}/>
              <Route exact path={ROUTES.WELCOME_SIGN_UP} render={() => <WelcomePage formTypeInitValue={"signUp"}/>}/>
              <Route exact path={ROUTES.HOME} component={HomePage}/>
              <Route exact path={ROUTES.GAME_HISTORY} component={GameHistory}/>
              <Route exact path={ROUTES.GAME} component={GamePage}/>
              <Route exact path={ROUTES.CONFIRM_ACCOUNT} component={ConfirmAccount}/>
          </MainLayout>
      </Switch>
    </div>
  );
};

export default App;
