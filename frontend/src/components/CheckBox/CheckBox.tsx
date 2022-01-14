import {Checkbox} from '@material-ui/core';
import s from './CheckBox.module.scss';

interface ICheckBox {
  value: boolean;
  handler: (event: any) => void;
  type: "primary" | "secondary" | "disabled"
} 

const CheckBox = ({ value, handler, type } : ICheckBox) => {
  return (
    <div className={s[type]}>
      <Checkbox
        color="primary"
        checked={value}
        onChange={handler}
        name="hintMode"
      ></Checkbox>
      <div>Hint mode</div>
    </div>
  );
};

export default CheckBox;
