import React, {useState, useEffect, ReactNode} from 'react';
import {useSelector, useDispatch} from 'react-redux';
import './MainLayout.sass';
import HeaderComponent from '../HeaderComponent/Header'

const MainLayout = ({children}: {children:ReactNode}) => {
  
  return (
    <div className="App stars-container">
      <div className="stars"></div>
      <div className="stars2"></div>
      <div className="stars3"></div>
      <HeaderComponent></HeaderComponent>
      {children}
    </div>
  );
};

export default MainLayout;
