import React from "react";
import { connect } from "react-redux";
import { Field, reduxForm } from "redux-form";

import { SaveButton, CancelButton } from "../_controls";

let NoteForm = props => {
  const { handleSubmit, onCancel } = props;
  return (
    <form className="noteForm" onSubmit={handleSubmit}>
      <div>
        <Field name="name" component="input" type="text" placeholder="Name" />
      </div>
      <div>
        <Field
          name="body"
          component="textarea"
          type="text"
          placeholder="note goes here"
        />
      </div>
      <CancelButton type="button" onClick={onCancel} />
      <SaveButton />
    </form>
  );
};

NoteForm = reduxForm({
  form: "NoteForm",
  enableReinitialize: true
})(NoteForm);

function mapStateToProps(state, props) {
  const { note } = props;

  return {
    initialValues: note
  };
}

export default connect(mapStateToProps)(NoteForm);
