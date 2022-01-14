import React, {useState, useEffect, Fragment} from 'react';
import {useSelector, useDispatch} from 'react-redux';
import {StylesProvider} from '@material-ui/core/styles';
import {Button} from '@material-ui/core';
import './MainMenu.scss';
import {useHistory} from 'react-router-dom';
import {ROUTES} from '../../core/_consts/routes';
const Menu = () => {
  const history = useHistory();
  return (
    <Fragment>
      <div className="menuContainerBase">
        <StylesProvider injectFirst>
          <div className="menuContainerInside">
            <Button
              variant="contained"
              color="primary"
              onClick={() => {
                history.push(ROUTES.GAME, {current: history.location.pathname});
              }}
            >
              Play
            </Button>
            <Button 
              variant="contained" 
              color="primary"
              onClick={() => {
                history.push(ROUTES.GAME_HISTORY, {current: history.location.pathname});
              }}>
              Game History
            </Button>
            {/* <Button variant="contained" color="primary">
              Ratings
            </Button>
            <Button variant="contained" color="primary">
              Profile
            </Button>
            <Button variant="contained" color="primary">
              Settings
            </Button> */}
          </div>
        </StylesProvider>
      </div>
    </Fragment>
  );
};

export default Menu;
