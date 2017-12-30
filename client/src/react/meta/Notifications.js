import React, { Component } from "react";
import { connect } from "react-redux";

class Notifications extends Component {
  render() {
    const { notifications } = this.props;
    return (
      <div className="notificationsContainer">
        {notifications.map((n, i) => {
          return (
            <div key={i}>
              {n.type} - {n.message}
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

export default connect(mapStateToProps)(Notifications);
