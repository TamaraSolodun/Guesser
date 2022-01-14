import React, { useState } from 'react';
import { useHistory } from 'react-router';
import { ROUTES } from '../../core/_consts/routes';
import SignIn from '../SignInComponent/SignIn';
import s from './WelcomePage.module.scss';

const WelcomePage = ({formTypeInitValue} : {formTypeInitValue:string}) => {
  return (
      <div className={s.welcomePageRoot}>
        <div className={s.welcomePageText} >GUESSER</div>
        <SignIn formType={formTypeInitValue} ></SignIn>
      </div>
  );
}

export default WelcomePage;
