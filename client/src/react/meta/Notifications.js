import React, { Component } from "react";
import { connect } from "react-redux";
import { metaActions } from "../../redux/actions";
import { TextButton } from "../_controls";

import "./Notifications.css";

class Notifications extends Component {
  render() {
    const {
      notifications,
      toggleRead,
      dismissNotification,
      readAll,
      dismissAll
    } = this.props;
    return (
      <div className="notificationsContainer">
        <TextButton onClick={() => readAll()}>Mark all Read</TextButton>
        <TextButton onClick={() => dismissAll()}>Dismiss all</TextButton>
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
    dismissNotification: obj => dispatch(metaActions.dismissNotification(obj)),
    readAll: () => dispatch(metaActions.markAllNotificationsRead()),
    dismissAll: () => dispatch(metaActions.dismissAllNotifications())
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Notifications);
