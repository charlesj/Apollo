import React, { Component } from "react";
import { connect } from "react-redux";
import { metaActions } from "../../redux/actions";
import { TextButton } from "../general";

import "./Notifications.css";

class Notifications extends Component {
  render() {
    const { notifications, toggleRead, dismissNotification } = this.props;
    return (
      <div className="notificationsContainer">
        {notifications.map((n, i) => {
          const className = "notification-" + n.type;
          return (
            <div key={i} className={className}>
              <strong>{n.message}</strong>
              <div>
                <TextButton
                  onClick={() => {
                    toggleRead(n);
                  }}
                >
                  Mark {n.unread ? "Read" : "Unread"}
                </TextButton>
                <TextButton
                  onClick={() => {
                    dismissNotification(n);
                  }}
                >
                  Dismiss
                </TextButton>
                {n.time.calendar()}
              </div>
            </div>
          );
        })}
      </div>
    );
  }
}

function mapStateToProps(state, props) {
  const { notifications } = state.meta;
  return {
    notifications
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    toggleRead: obj => dispatch(metaActions.toggleNotificationRead(obj)),
    dismissNotification: obj => dispatch(metaActions.dismissNotification(obj))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Notifications);
