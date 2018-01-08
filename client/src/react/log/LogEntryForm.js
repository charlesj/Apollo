import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { Field, reduxForm } from "redux-form";
import { CancelButton, SaveButton, Container } from "../_controls";

class LogEntryForm extends React.Component {
  render() {
    const { handleSubmit, onCancel } = this.props;
    return (
      <Container className="logEntryFormContainer" width={400}>
        <form onSubmit={handleSubmit}>
          <div>
            <Field
              name="note"
              component="textarea"
              type="text"
              placeholder="note"
            />
          </div>
          <div>
            <Field
              name="unifiedTags"
              component="input"
              type="text"
              placeholder="Tags"
            />
          </div>
          <CancelButton onClick={onCancel} />
          <SaveButton type="submit" primary />
        </form>
      </Container>
    );
  }
}

LogEntryForm.propTypes = {
  onSubmit: PropTypes.func.isRequired
};

LogEntryForm = reduxForm({
  form: "logEntryForm",
  enableReinitialize: true
})(LogEntryForm);

function mapStateToProps(state, props) {
  return {
    initialValues: {}
  };
}

export default connect(mapStateToProps)(LogEntryForm);
