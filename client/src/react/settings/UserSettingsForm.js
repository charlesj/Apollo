import React from "react";
import { connect } from "react-redux";
import { Field, reduxForm } from "redux-form";

import { SaveButton, CancelButton } from "../_controls";

let UserSettingsForm = props => {
  const { handleSubmit, onCancel } = props;
  return (
    <form className="userSettingsForm" onSubmit={handleSubmit}>
      <div>
        <Field name="name" component="input" type="text" placeholder="Name" />
      </div>
      <div>
        <Field name="value" component="input" type="text" placeholder="Value" />
      </div>
      <CancelButton type="button" onClick={onCancel} />
      <SaveButton />
    </form>
  );
};

UserSettingsForm = reduxForm({
  form: "UserSettingsForm",
  enableReinitialize: true
})(UserSettingsForm);

function mapStateToProps(state, props) {
  const { setting } = props;

  return {
    initialValues: setting
  };
}

export default connect(mapStateToProps)(UserSettingsForm);
