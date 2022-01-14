import { UserStatus } from '../../../core/_consts/enums';
import {CREATE_USER, DELETE_USER, GET_USER, GET_USERS, UPDATE_USER, LOGIN_USER, SHOW_ALERT, ACTIVATE_USER} from '../../action-types';

//#region Interfaces
export interface IUser {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  nickName: string;
  email: string;
}

export interface ICreateUserBody {
  firstName: string;
  lastName: string;
  nickName: string;
  email: string;
  password: string;
}

export interface ILogginedUser {
  token: string;
  userInfo: IUser;
}
export interface ILoginUserBody {
  email: string;
  password: string;
}

export interface IActivateUserBody {
  id: number;
  tokenToActivate: string;
  userStatus: UserStatus;
}

export interface UserConfig {
  email?: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  nickName?: string;
  currentPage: number;
  itemsOnPage: number;
  sortingDirection: boolean;
}
export interface IAlert {
  type: 'success' | 'info' | 'warning' | 'error' | undefined;
  message: string;
}
//#endregion

//#region Actions
export interface ICreateUser {
  type: typeof CREATE_USER;
  payload: IUser;
}
export interface IActivateUser {
  type: typeof ACTIVATE_USER;
  payload: boolean;
}
export interface IShowAlert{
  type: typeof SHOW_ALERT;
  payload: IAlert;
}
export interface ILoginUser {
  type: typeof LOGIN_USER;
  payload: ILogginedUser;
}
export interface IGetUser {
  type: typeof GET_USER;
  payload: IUser;
}
export interface IGetUsers {
  type: typeof GET_USERS;
  payload: Array<IUser>;
}
export interface IDeleteUser {
  type: typeof DELETE_USER;
  payload: boolean;
}

// export interface IUpdateUser {
//     type: typeof UPDATE_USER;
//     payload: IUser;
// }

//#endregion

//#region reducer
export interface IUserReducer {
  user: IUser;
  alert: IAlert;
}
//#endregion

export type UserActions = ICreateUser | ILoginUser | IGetUser | IGetUsers | IDeleteUser | IActivateUser | IShowAlert;
