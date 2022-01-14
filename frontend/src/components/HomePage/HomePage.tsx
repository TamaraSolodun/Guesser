import React from 'react';
import logo from './logo.svg';
import './HomePage.css';
import Menu from '../MainMenu/MainMenu';

function HomePage() {
  return (
    <div>
      <div className="mainLogo">
        <div style={{backgroundImage: `url(${logo})`}} className="App-logo"></div>
      </div>
      <div className="menu-container-base root-container">
        <Menu />
      </div>
    </div>
  );
}

export default HomePage;
