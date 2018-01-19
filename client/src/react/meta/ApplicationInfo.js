import React, { Component } from "react";
import { connect } from "react-redux";
import moment from "moment";

import { metaActions} from "../../redux/actions";

class ApplicationInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      compiledDisplay: null
    };
  }

  componentWillMount(){
    this.props.load();
  }

  componentDidMount() {
    var intervalId = setInterval(() => this.updateDisplay(), 1000);
    this.setState({ intervalId: intervalId });
  }

  updateDisplay() {
    if (this.props.applicationInfo) {
      const { compiledOn } = this.props.applicationInfo;
      if (compiledOn) {
        this.setState({ compiledDisplay: moment(compiledOn).calendar() });
      }
    }
  }

  componentWillUnmount() {
    clearInterval(this.state.intervalId);
  }

  render() {
    const { version, commitHash } = this.props.applicationInfo;
    const { compiledDisplay } = this.state;

    return (
      <div className="applicationInfo">
        Apollo <strong>{version}</strong> ({commitHash &&
          commitHash.slice(0, 6)}) - {compiledDisplay}
      </div>
    );
  }
}
function mapStateToProps(state, props) {
  const { applicationInfo } = state.meta;

  return {
    applicationInfo
  };
}

function mapDispatchToProps(dispatch, props){
  return {
    load: () => dispatch(metaActions.applicationInfo()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(ApplicationInfo);
