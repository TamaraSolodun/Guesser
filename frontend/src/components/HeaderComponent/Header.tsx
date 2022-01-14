import {Avatar, Button} from '@material-ui/core';
import React, {Fragment, useEffect, useState} from 'react';
import s from './Header.module.scss';
import {ROUTES} from '../../core/_consts/routes';
import {useHistory, useLocation} from 'react-router-dom';
import CheckBox from '../CheckBox/CheckBox';
import {connect, ConnectedProps} from 'react-redux';
import {selectorGetSettings} from '../../redux/settings/selector';
import {Store} from '../../redux/root';
import {updateSettings} from '../../redux/settings/actions';
import {VaultService} from '../../utils/storage';
import {IUser} from '../../redux/user/interfaces';

interface LocationState {
  current: string;
}

const HeaderComponent = ({settings, updateSettings}: HeaderProps) => {
  let history = useHistory();
  const location = useLocation<LocationState>();
  console.log(location);
  const [path, setPath] = useState(location.pathname);
  const [isHintModeEnabled, setIsHintModeEnabled] = useState(settings.isHintModeEnabled);
  const handleHintModeChange = (event: any) => {
    setIsHintModeEnabled(event.target.checked);
    updateSettings({difficult: settings.difficult, isHintModeEnabled: event.target.checked});
  };
  // if(window.location.href !== path){
  //   setPath(window.location.href);
  // }

  useEffect(() => {
    setPath(location.pathname);
  }, [location.pathname]);
  const storage = new VaultService();
  const user: IUser = storage.getItem('currentUser');
  const logout = () => {
    storage.clearStorage();
    history.push(ROUTES.WELCOME, {current: history.location.pathname});
  };
  return (
    <Fragment>
      {path === '/' || path === "/sign-up" ? null : (
        <div className={s.headerContainer}>
          <div className={s.headerLeftSide}>
            {path.includes('/home') ? null : (
              <Button
                className={s.headerButton}
                variant="contained"
                color="primary"
                onClick={() => {
                  history.push(ROUTES.HOME, {current: history.location.pathname});
                }}
              >
                Menu
              </Button>
            )}
            {path === '/game' ? (
              <CheckBox value={isHintModeEnabled} handler={handleHintModeChange} type={'primary'}></CheckBox>
            ) : null}
            {/* <Button></Button> */}
          </div>

          <div className={s.headerRightSide}>
            <Button className={s.avatarButton}>
              <Avatar
                alt="Remy Sharp"
                className={s.avatar}
                src="https://i.pinimg.com/originals/9c/77/46/9c7746225873e02d83b9315501b8dd2f.jpg"
              />
            </Button>
            {!!user?.nickName && <div className={s.nickName}>{user.nickName}</div>}
            <Button className={s.headerButton} variant="contained" color="primary" onClick={logout}>
              Logout
            </Button>
          </div>
        </div>
      )}
    </Fragment>
  );
};

const mapStateToProps = (state: Store) => ({
  settings: selectorGetSettings(state),
});

const mapDispatchToProps = {
  updateSettings,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type HeaderProps = ConnectedProps<typeof connector>;

export default connector(HeaderComponent);
