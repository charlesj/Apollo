import React from "react";
import { connect } from "react-redux";
import { metaSelectors } from "../../redux/selectors";
import FontAwesome from "react-fontawesome";

function ServerActivity(props) {
  if (props.isRequesting) {
    return (
      <div>
        <FontAwesome name="cog" spin />
      </div>
    );
  }
  return (
    <div>
      <FontAwesome name="cog" />
    </div>
  );
}

function mapStateToProps(state, props) {
  return {
    isRequesting: metaSelectors.activeServerRequests(state)
  };
}

export default connect(mapStateToProps)(ServerActivity);
