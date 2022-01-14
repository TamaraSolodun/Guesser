import axios from 'axios';
import {VaultService} from './storage';
import {ROUTES} from '../core/_consts/routes';

export const storageKey = {
    user: 'user',
    token: 'token',
  };
const vaultService = new VaultService();
const axiosConfig = {
  baseURL: 'http://localhost/api/',
};
// https://dynamitisecureportal.intetics.com:2800/api/
const instance = axios.create(axiosConfig);

// Interceptors
instance.interceptors.request.use(
  (req) => {
    const TOKEN = vaultService.getItem(storageKey.token);

    if (!!TOKEN) {
      Object.assign(req.headers, {Authorization: TOKEN});
    }

    return req;
  },
  (error) => {
    return Promise.reject(error.response);
  }
);

//on successful response
instance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error && error.response && error.response.status === 401) {
      window.location.href = ROUTES.WELCOME;
      vaultService.clearStorage();
      sessionStorage.clear();
    } else {
      return Promise.reject(error.response);
    }
  }
);

export default instance;