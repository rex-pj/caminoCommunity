import gpl from "graphql-tag";

export const ADD_USER = gpl`
  mutation Adduser($user: RegisterInputType!){
    adduser(user: $user){
      succeeded,
      errors {
        code,
        description
      }
    }
  }
`;
