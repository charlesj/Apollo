import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function CancelButton(props) {
  const { className } = props;
  return (
    <Button {...props} type="button" className={ClassNames(className, 'cancelButton')}>
      <FontAwesome name="ban" /> Cancel
    </Button>
  );
}

export default CancelButton;
