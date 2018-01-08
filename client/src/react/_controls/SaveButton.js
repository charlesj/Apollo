import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function SaveButton(props) {
  const { className } = props;
  return (

    <Button {...props} className={ClassNames(className, 'saveButton')}>
      <FontAwesome name="floppy-o" /> Save
    </Button>
  );
}

export default SaveButton;
