import React, { Component } from 'react';
import axios from 'axios';

export class JwtBearerClient extends Component{
    constructor(props) {
        super(props);
        this.state = { 
            jwt_token:"none"
        };
      }

  componentDidMount() {
      this.getToken();
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
      console.log(data);
      this.forceUpdate();
      return data;
  }

  render() {

    return (
      <div>
        <p>Hi</p>
        <p>Current JWT Token: {this.state.jwt_token}</p>
      </div>
    );
  }
}