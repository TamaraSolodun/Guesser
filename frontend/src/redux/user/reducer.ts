import {CREATE_USER, DELETE_USER, GET_USER, GET_USERS, LOGIN_USER, SHOW_ALERT} from '../action-types';
import {IAlert, ILogginedUser, IUser, IUserReducer, UserActions} from './interfaces';
import {VaultService} from '../../utils/storage';

const storage = new VaultService();

const userInitState: IUserReducer = {
  user: {
    id: 0,
    firstName: '',
    lastName: '',
    fullName: '',
    nickName: '',
    email: '',
  },
  alert: {
    type: undefined,
    message: '',
  },
};

export const userReducer = (state = userInitState, actions: UserActions): IUserReducer => {
  const {type, payload} = actions;
  switch (type) {
    case GET_USER:
      return {...state, user: payload as IUser};
    // case GET_USERS:
    //   break;
    case CREATE_USER:
      return {...state, user: payload as IUser};
    // case DELETE_USER:
    //   break;
    case LOGIN_USER:
      storage.setItem('token', (payload as ILogginedUser).token);
      storage.setItem('currentUser', (payload as ILogginedUser).userInfo);
      return {...state, user: (payload as ILogginedUser).userInfo};
    case SHOW_ALERT:
      return {...state, user: state.user, alert: payload as IAlert};
    default:
      return state;
  }
};

export default userReducer;
