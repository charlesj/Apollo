import React, { Component } from "react";
import { connect } from "react-redux";
import { goalActions } from "../../redux/actions";
import { TextButton, FlexRow, Container } from "../general";
import FontAwesome from "react-fontawesome";
import GoalForm from "./GoalForm";

import "./Goals.css";

class Goals extends Component {
  constructor(props) {
    super(props);
    this.state = {
      editingGoal: null
    };

    this.onSubmit = this.onSubmit.bind(this);
  }
  newGoal() {
    return {
      slug: "",
      title: "",
      description: "",
      startDate: "",
      endDate: "",
      metricName: "",
      targetValue: 0.0,
      completed: false,
      featured: false
    };
  }

  onSubmit(goal) {
    goal.featured = goal.featured === "true";
    const { upsertGoal } = this.props;
    upsertGoal(goal);
    this.setEditingGoal(null);
  }

  setEditingGoal(editingGoal) {
    this.setState({
      editingGoal
    });
  }

  render() {
    const { goals } = this.props;

    return (
      <FlexRow>
        <Container>
          Goals
          {goals.map(g => {
            return (
              <div key={g.slug} className="goalListing">
                {g.title || g.slug}
                <TextButton
                  onClick={() => {
                    this.setEditingGoal(g);
                  }}
                >
                  <FontAwesome name="edit" />
                </TextButton>
              </div>
            );
          })}
          <TextButton
            onClick={() => {
              this.setEditingGoal(this.newGoal());
            }}
          >
            New Goal
          </TextButton>
        </Container>
        {this.state.editingGoal && (
          <Container>
            <GoalForm
              goal={this.state.editingGoal}
              onSubmit={this.onSubmit}
              onCancel={() => {
                this.setEditingGoal(null);
              }}
            />
          </Container>
        )}
      </FlexRow>
    );
  }
}

function mapStateToProps(state, props) {
  const { goals } = state.goals;

  return {
    goals
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    upsertGoal: goal => dispatch(goalActions.upsertGoal(goal))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Goals);
