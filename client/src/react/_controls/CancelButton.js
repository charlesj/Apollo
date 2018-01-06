import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";

function CancelButton(props) {
  return (
    <Button {...props}>
      <FontAwesome name="ban" /> Cancel
    </Button>
  );
}

export default CancelButton;
