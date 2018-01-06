import React, { Component } from "react";
import { connect } from "react-redux";

import { FlexRow, TextButton } from "../_controls";
import { metaSelectors } from "../../redux/selectors";
import ServerActivity from "./ServerActivity";
import "./MenuBar.css";

class MenuBar extends Component {
  render() {
    const {
      unreadNotificationCount,
      toggleTerminal,
      latestMessage
    } = this.props;
    return (
      <FlexRow className="menuBar">
        <ServerActivity />
        <TextButton onClick={toggleTerminal}>Terminal</TextButton>

        {unreadNotificationCount > 0 && (
          <div>
            {unreadNotificationCount} new update: {latestMessage}
          </div>
        )}
      </FlexRow>
    );
  }
}

function mapStateToProps(state, props) {
  const latestNotification = metaSelectors.latestNotification(state);
  return {
    unreadNotificationCount: metaSelectors.unreadNotificationCount(state),
    latestMessage: latestNotification ? latestNotification.message : null
  };
}

export default connect(mapStateToProps)(MenuBar);
