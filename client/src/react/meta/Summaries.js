import React, { Component } from 'react';
import { connect } from "react-redux";
import { summaryActions } from "../../redux/actions";
import { Card, FlowRow } from "../general/";


class Summaries extends Component {

  componentWillMount(){
    this.props.getSummaries();
  }

  render(){
    const { summaries } = this.props;
    return (<FlowRow>
      {summaries.map(summary => {
        return <Card title={summary.label} content={summary.amount} key={summary.id} />
      })}
    </FlowRow>)
  }
}

function mapStateToProps(state, props){
  const { summaries } = state.summaries;
  return {
    summaries
  };
}

function mapDispatchToProps(dispatch, props){
  return {
    getSummaries: () => dispatch(summaryActions.getSummaries())
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Summaries);
