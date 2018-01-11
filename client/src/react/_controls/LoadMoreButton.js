import React from "react";
import Button from "./Button";
import FontAwesome from "react-fontawesome";
import ClassNames from "classnames";

function LoadMoreButton(props) {
  const { className } = props;
  return (
    <Button
      {...props}
      primary
      className={ClassNames(className, "loadMoreButton")}
    >
      <FontAwesome name="chevron-circle-right" /> Load More
    </Button>
  );
}

export default LoadMoreButton;
