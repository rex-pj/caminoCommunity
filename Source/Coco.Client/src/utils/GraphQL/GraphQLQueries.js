import gpl from "graphql-tag";

export const ADD_USER = gpl`
  mutation Adduser($user: RegisterInputType!){
    adduser(user: $user){
      isSuccess,
      errors {
        code,
        description
      }
    }
  }
`;

export const SIGNIN = gpl`
query signin($signinModel: SigninInputType!){
  signin(signinModel: $signinModel){
    userInfo {
      displayName,
      userHashedId
    },
    authenticatorToken,
    isSuccess,
    errors {
      code
      description
    }
  }
}
`;

export const GET_LOGGED_USER = gpl`
query getLoggedUser {
  getLoggedUser {
    userInfo {
      displayName,
      userHashedId
    },
    authenticatorToken,
    isSuccess,
    errors {
      code
      description
    }
  }
}
`;
