import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function RemoveButton(props) {
  const { className } = props;
  return (
    <Button
      {...props}
      type="button"
      className={ClassNames(className, "removeButton")}
    >
      <FontAwesome name="trash" /> Remove
    </Button>
  );
}

export default RemoveButton;
