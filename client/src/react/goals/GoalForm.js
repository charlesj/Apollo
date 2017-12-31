import React from "react";
import { connect } from "react-redux";
import { Field, reduxForm } from "redux-form";

let GoalForm = props => {
  const { handleSubmit, onCancel } = props;
  return (
    <form className="goalEditContainer" onSubmit={handleSubmit}>
      <h1>Editing Goal</h1>
      <label>Slug</label>
      <div>
        <Field name="slug" component="input" type="text" placeholder="slug" />
      </div>
      <label>Title</label>
      <div>
        <Field
          name="title"
          component="input"
          type="text"
          placeholder="Goal Title"
        />
      </div>
      <label>Description</label>
      <div>
        <Field
          name="description"
          component="input"
          type="text"
          placeholder="Description"
        />
      </div>
      <label>Start Date</label>
      <div>
        <Field
          name="startDate"
          component="input"
          type="date"
          placeholder="Start Date"
        />
      </div>
      <label>End Date</label>
      <div>
        <Field
          name="endDate"
          component="input"
          type="date"
          placeholder="End Date"
        />
      </div>
      <label>Metric Name</label>
      <div>
        <Field
          name="metricName"
          component="input"
          type="text"
          placeholder="Metric Name"
        />
      </div>
      <label>Target Value</label>
      <div>
        <Field
          name="targetValue"
          component="input"
          type="text"
          placeholder="Target value"
        />
      </div>
      <label>Featured</label>
      <div>
        <Field name="featured" component="input" type="text" />
      </div>
      <button type="submit">Submit</button>
      <button type="button" onClick={onCancel}>
        Cancel
      </button>
    </form>
  );
};

GoalForm = reduxForm({
  form: "GoalForm"
})(GoalForm);

function mapStateToProps(state, props) {
  const { goal } = props;

  return {
    initialValues: goal
  };
}

export default connect(mapStateToProps)(GoalForm);
