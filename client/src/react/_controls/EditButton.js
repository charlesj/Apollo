import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function EditButton(props) {
  const { className, small } = props;
  return (
    <Button
      {...props}
      type="button"
      className={ClassNames({
        [className]: true,
        editButton: true,
        smallButton: small
      })}
      title="edit"
    >
      <span title="edit">
        <FontAwesome name="edit" />
        {!small && " Edit"}
      </span>
    </Button>
  );
}

export default EditButton;
