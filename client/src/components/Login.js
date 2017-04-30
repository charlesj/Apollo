var React = require('react');

var apollo = require('../services/apollo-server');

class Login extends React.Component{
  constructor(props){
    super(props);

    this.state = {
      password: '',
      showWrong: false,
      attempts: 0
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    this.setState({
      password: event.target.value
    });
  }

  handleSubmit(event) {
    event.preventDefault();

    apollo.invoke("Login", { password: this.state.password })
      .then((data) => {
        if(data.token != null){
          this.setState({
            password: ''
          });
          this.props.onLogin(data.token);
        } else {
          this.setState({
            showWrong: true,
            attempts: this.state.attempts + 1
          });
        }
      }).catch(err => {
        this.setState({
          showWrong: true,
          attempts: this.state.attempts + 1
        });
      });
  }

  render(){
    return(<div className="container-fluid" id="login">
        <form onSubmit={this.handleSubmit} className="form-inline">
          <div className="form-group">
            <div className="input-group">
              <div className="input-group-addon input-sm">Login</div>
              <input
                type='password'
                id='password'
                className='form-control input-sm'
                value={this.state.password}
                onChange={this.handleChange}/>
            </div>
          </div>
        </form>
        {this.state.showWrong && <div className='bg-danger loginBad'>Wrong password ({this.state.attempts} attempts)</div>}
    </div>)
  }
}

module.exports = Login
