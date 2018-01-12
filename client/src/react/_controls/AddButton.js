import React from "react";
import TextButton from "./TextButton";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function AddButton(props) {
  const { className, noun, ...rest } = props;
  return (
    <TextButton
      {...rest}
      type="button"
      className={ClassNames(className, "addButton")}
    >
      <FontAwesome name="plus" /> Add {noun}
    </TextButton>
  );
}

export default AddButton;
