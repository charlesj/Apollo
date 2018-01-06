import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";

function SaveButton(props) {
  return (
    <Button {...props}>
      <FontAwesome name="floppy-o" /> Save
    </Button>
  );
}

export default SaveButton;
