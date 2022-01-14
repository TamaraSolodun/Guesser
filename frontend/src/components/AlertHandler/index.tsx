import {Snackbar} from '@material-ui/core';
import React, {useEffect, useState} from 'react';
import {connect, ConnectedProps} from 'react-redux';
import Alert from '../Alert';
import {showAlert} from '../../redux/user/actions';
import {IAlert} from '../../redux/user/interfaces';
import {Store} from '../../redux/root';

const AlertHandler: React.FC<AlertProps> = ({alert}: AlertProps) => {
  const [open, setOpen] = useState(false);

  const handleClose = (): void => {
    setOpen(false);
  };

  useEffect(() => {
    setOpen(!!alert.message);
  }, [alert]);

  return (
    <Snackbar open={open} autoHideDuration={3000} onClose={handleClose}>
      <Alert severity={alert.type}>{alert.message}</Alert>
    </Snackbar>
  );
};

// Redux
const mapStateToProps = (store: Store): {alert: IAlert} => {
  return {
    alert: store.userRedux.alert,
  };
};

const mapDispatchToProps = {
  showAlert,
};

const connector = connect(mapStateToProps, mapDispatchToProps);

type AlertProps = ConnectedProps<typeof connector>;

export default connector(AlertHandler);
