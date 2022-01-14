import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router';
import { useLocation } from 'react-router-dom';
import { ROUTES } from '../../core/_consts/routes';
import s from './ConfirmAccount.module.scss';
import {activateUser} from '../../redux/user/actions' 
import { Store } from '../../redux/root';
import { connect } from 'react-redux';
import { UserStatus } from '../../core/_consts/enums';
function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const ConfirmAccount = ({activateUser} : any) => {
  const history = useHistory();
  let query = useQuery();
  useEffect(() => {
    activateUser({id: Number(query.get("id")), tokenToActivate: query.get("token"), userStatus: UserStatus.Active});
    history.push(ROUTES.WELCOME);
  })
  console.log(query.get("token"));
  return (
      <div className={s.welcomePageRoot}>
        <div className={s.welcomePageText}>Account successfully confirmed!</div>
      </div>
  );
}
const mapStateToProps = (state: Store) => ({
});
const mapDispatchToProps = {
  activateUser
}
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmAccount);
