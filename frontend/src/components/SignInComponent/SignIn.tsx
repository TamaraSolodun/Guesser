import {Button, TextField} from '@material-ui/core';
import React, {Fragment, SyntheticEvent, useState} from 'react';
import { connect, ConnectedProps } from 'react-redux';
import s from './SignIn.module.scss';
import {createUser, loginUser, showAlert} from "../../redux/user/actions"
import { Store } from '../../redux/root';
import { ICreateUserBody } from '../../redux/user/interfaces';
import { ROUTES } from '../../core/_consts/routes';
import { useHistory } from 'react-router-dom';

function isNumericChar(c:string) { return /\d/.test(c); }
function passwordValidate(password:string) {
  let splitedPass = password.split('');
  if (splitedPass.length < 8){
    return false;
  }
  else{
    if (splitedPass.some(i => i.toUpperCase() == i) && splitedPass.some(i => i.toLowerCase() == i) && splitedPass.some(i => isNumericChar(i))){
      return true;
    }
    return false;
  }
}


const SignIn: React.FC<SignInProps> = ({formType, loginUser, createUser, showAlert}) => {
  const [user, setUser] = useState<ICreateUserBody>({
    firstName: '',
    lastName: '',
    nickName: '',
    email: '',
    password: '',
  });
  const [errors, setErrors] = useState({
    emailError: 'Email should be not empty!',
    firstNameError: 'First Name should be not empty!',
    lastNameError: 'Last Name should be not empty!',
    nickNameError: 'Nick Name should be not empty!',
    passwordError: 'Password should be at least 8 symbols length, contains upper and lower case letters and numeric symbols!',
  });
  const setUserObject = (event: any) => {
    setUser((prevState) => {
      return {...prevState, [event.target.name]: event.target.value};
    });
    switch (event.target.name) {
      case 'email':
        if (!event.target.value) {
          setErrors((prevState) => {
            return {...prevState, emailError: 'Email should be not empty!'};
          });
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(event.target.value)) {
          setErrors((prevState) => {
            return {...prevState, emailError: 'Email should be valid!'};
          });
        } else {
          setErrors((prevState) => {
            return {...prevState, emailError: ''};
          });
        }
        break;
      case 'firstName':
        if (!event.target.value) {
          setErrors((prevState) => {
            return {...prevState, firstNameError: 'First Name should be not empty!'};
          });
        }
        else {
          setErrors((prevState) => {
            return {...prevState, firstNameError: ''};
          });
        }
        break;
      case 'lastName':
        if (!event.target.value) {
          setErrors((prevState) => {
            return {...prevState, lastNameError: 'Last Name should be not empty!'};
          });
        }
        else {
          setErrors((prevState) => {
            return {...prevState, lastNameError: ''};
          });
        }
        break;
      case 'nickName':
        if (!event.target.value) {
          setErrors((prevState) => {
            return {...prevState, nickNameError: 'First Name should be not empty!'};
          });
        }
        else{
          setErrors((prevState) => {
            return {...prevState, nickNameError: ''};
          });
        }
        break;
      case 'password':
        if (!event.target.value) {
          setErrors((prevState) => {
            return {...prevState, passwordError: 'Password should be not empty!'};
          });
        }
        else if (!passwordValidate(event.target.value)) {
          setErrors((prevState) => {
            return {...prevState, passwordError: 'Password should be at least 8 symbols length, contains upper and lower case letters and numeric symbols!'};
          });
        }
        else{
          setErrors((prevState) => {
            return {...prevState, passwordError: ''};
          });
        }
        break;
    }
  };
  const history = useHistory();
  const setFormType = (value:string) => {
    if (value == "signIn"){
      history.push(ROUTES.WELCOME);
    }
    else{
      history.push(ROUTES.WELCOME_SIGN_UP);
    }
  }
  const sign = async(event:SyntheticEvent) =>{
    if (formType == "signIn"){
      try{
        await loginUser({email: user.email, password: user.password});
        history.push(ROUTES.HOME, {current: history.location.pathname});
      }
      catch (error){
      }
    }
    else{
      try{
        await createUser(user);
        history.push(ROUTES.WELCOME, {current: history.location.pathname});
      }
      catch (error){
      }
    }
  }
  return (
    <div className={s.signInContainer}>
      <div className={s.header}>
        <h1>{formType === 'signIn' ? 'Sign In' : 'Sign Up'}</h1>
      </div>
      <div className={s.content}>
        {formType === 'signIn' ? (
          <Fragment>
            <TextField
              className={s.guessInput}
              label="Email"
              variant="filled"
              type="email"
              size="small"
              name="email"
              value={user.email}
              onChange={(i) => setUserObject(i)}
              helperText={errors.emailError}
            />
            <TextField
              className={s.guessInput}
              label="Password"
              variant="filled"
              type="password"
              size="small"
              name="password"
              value={user.password}
              onChange={(i) => setUserObject(i)}
              helperText={errors.passwordError}
            />
          </Fragment>
        ) : (
          <Fragment>
            <TextField
              className={s.guessInput}
              label="First Name"
              variant="filled"
              type="text"
              size="small"
              name="firstName"
              value={user.firstName}
              onChange={(i) => setUserObject(i)}
              helperText={errors.firstNameError}
            />
            <TextField
              className={s.guessInput}
              label="Last Name"
              variant="filled"
              type="text"
              size="small"
              name="lastName"
              value={user.lastName}
              onChange={(i) => setUserObject(i)}
              helperText={errors.lastNameError}
            />
            <TextField
              className={s.guessInput}
              label="Nick Name"
              variant="filled"
              type="text"
              size="small"
              name="nickName"
              value={user.nickName}
              onChange={(i) => setUserObject(i)}
              helperText={errors.nickNameError}
            />
            <TextField
              className={s.guessInput}
              label="Email"
              variant="filled"
              type="email"
              size="small"
              name="email"
              value={user.email}
              onChange={(i) => setUserObject(i)}
              helperText={errors.emailError}
            />
            <TextField
              className={s.guessInput}
              label="Password"
              variant="filled"
              type="password"
              size="small"
              name="password"
              value={user.password}
              onChange={(i) => setUserObject(i)}
              helperText={errors.passwordError}
            />
          </Fragment>
        )}
      </div>
      <div className={s.footer}>
        <Button
          className={s.guessButton}
          variant="contained"
          color="primary"
          onClick={sign}
          disabled={formType == "signIn" ? ((errors.emailError || errors.passwordError) ? true : false) : Object.values(errors).some((i) => i !== '')}
        >
          {formType === 'signIn' ? 'Sign In' : 'Sign Up'}
        </Button>
        <Button
          className={s.guessButton}
          variant="contained"
          color="primary"
          onClick={(i) => {
            setFormType(formType === 'signIn' ? 'signUp' : 'signIn');
          }}
        >
          {formType === 'signIn' ? 'Go to Sign Up' : 'Go to Sign In'}
        </Button>
      </div>
    </div>
  );
};
const mapStateToProps = (state: Store) => ({
});
const mapDispatchToProps = {
  loginUser,
  createUser,
  showAlert
}

const connector = connect(mapStateToProps, mapDispatchToProps);
type SignInProps = ConnectedProps<typeof connector> & {
  formType: string
};
export default connector(SignIn);
