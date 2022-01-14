import {Dispatch} from '@reduxjs/toolkit';
import {useHistory} from 'react-router';
import {USER} from '../../core/_consts/api-urls';
import {ROUTES} from '../../core/_consts/routes';
import axios from '../../utils/API';
import {ACTIVATE_USER, CREATE_USER, DELETE_USER, GET_USER, GET_USERS, LOGIN_USER, SHOW_ALERT} from '../action-types';
import {IActivateUserBody, IAlert, ICreateUserBody, ILogginedUser, ILoginUserBody, IShowAlert, IUser, UserConfig} from './interfaces';

export const getUser = () => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.get(USER.GET_BY_ID);
    dispatch({type: GET_USER, payload: response.data});
  } catch (error) {
    console.warn(error);
  }
};
export const getUsers = (userConfig: UserConfig) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(USER.GET_USERS, userConfig);
    dispatch({type: GET_USERS, payload: response.data});
  } catch (error) {
    console.warn(error);
  }
};
export const createUser = (userToCreate: ICreateUserBody) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(USER.CREATE, userToCreate);
    dispatch({type: CREATE_USER, payload: response.data});
    dispatch(showAlert({type: "success", message: "Registered successful! Check your email to activate account."}));
  } catch (error) {
    dispatch(showAlert({type: "error", message: error.data.detail}));
    throw error;
  }
};
export const deleteUser = (userId: number) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.delete(USER.DELETE(userId));
    dispatch({type: DELETE_USER, payload: response.data});
  } catch (error) {
    console.warn(error);
  }
};
export const loginUser = (loginUserBody: ILoginUserBody) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(USER.LOGIN, loginUserBody);
    dispatch({type: LOGIN_USER, payload: response.data as ILogginedUser});
    dispatch(showAlert({type: "success", message: "Login successful!"}));
  } catch (error) {
    dispatch(showAlert({type: "error", message: error.data.detail}));
    throw error;
  }
};

export const activateUser = (activateUserBody: IActivateUserBody) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(USER.CONFIRM_ACCOUNT, activateUserBody);
    //dispatch({type: ACTIVATE_USER, payload: response.data});
    dispatch(showAlert({type: "success", message: "Account activated!"}));
  } catch (error) {
    dispatch(showAlert({type: "error", message: error.data.detail}));
    throw error;
  }
};

export const showAlert = (payload: IAlert): IShowAlert => {
  return {
    type: SHOW_ALERT,
    payload,
  };
};