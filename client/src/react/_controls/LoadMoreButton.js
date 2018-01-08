import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";

function LoadMoreButton(props) {
  return (
    <Button {...props} primary>
      <FontAwesome name="chevron-circle-right" /> Load More
    </Button>
  );
}

export default LoadMoreButton;
