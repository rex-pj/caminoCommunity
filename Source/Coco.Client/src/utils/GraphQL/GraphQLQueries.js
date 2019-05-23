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

export const Signin = gpl`
query signin($signinModel: SigninInputType!){
  signin(signinModel: $signinModel){
    token
  }
}
`;
