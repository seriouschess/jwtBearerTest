import React, { Component } from 'react';
import axios from 'axios';

export class JwtBearerClient extends Component{
    constructor(props) {
        super(props);
        this.state = { 
            jwt_token:"none",
            authentication_response:"untested"
        };
      }

  componentDidMount() {
      let token = this.getToken();
      
      this.authenticateSecureEndpoint(token);
  }

  async getToken() {
    let content = {
        email:"fake@notreal.com",
        password:"12345"
    }
    const data = await axios.post('main/token/get', content).then( res => {
        return res.data;
      });
      this.setState( { jwt_token:data.token } );

      this.forceUpdate();
      return await data.token;
  }

  async authenticateSecureEndpoint(token_parameter){ //token_parameter passed in as promise
  console.log(await token_parameter);

  const data = await axios.get('main/authenticate', {
    headers: {
      'Authorization': `Bearer ${ await token_parameter }`
    }
  }).then((res) => { return res.data}
  ).catch((err) => {console.log(err)});
  this.setState({ authentication_response:data });

  //Referrer Policystrict-origin-when-cross-origin ??

    this.setState( { authentication_response:data } );
    this.forceUpdate();
    return data;
}

  render() {

    return (
      <div>
        <p>Hi</p>
        <p>Current JWT Token: {this.state.jwt_token}</p>
        <p>Authentication response: {this.state.authentication_response}</p>
      </div>
    );
  }
}