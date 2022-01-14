import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import store from './redux/root';
import {Provider} from 'react-redux';
import {BrowserRouter} from 'react-router-dom';
import App from './App';

ReactDOM.render(
  <BrowserRouter>
    <Provider store={store}>
      <App />
    </Provider>
  </BrowserRouter>,
  document.getElementById('root')
);
