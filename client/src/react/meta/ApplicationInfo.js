import React from "react";
import { connect } from "react-redux";
import moment from "moment";

function ApplicationInfo(props) {
  const { version, commitHash, compiledOn } = props.applicationInfo;
  const compiledDisplay = moment(compiledOn).calendar();

  return (
    <div className="applicationInfo">
      Apollo <strong>{version}</strong> ({commitHash && commitHash.slice(0, 6)})
      - {compiledDisplay}
    </div>
  );
}

function mapStateToProps(state, props) {
  const { applicationInfo } = state.meta;

  return {
    applicationInfo
  };
}

export default connect(mapStateToProps)(ApplicationInfo);
