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
  query{
    loggedUser{
      lastname,
      firstname,
      email,
      displayName,
      userHashedId
    }
  }
`;

export const GET_USER_INFO = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      email,
      displayName,
      userHashedId,
      address,
      birthDate,
      countryId,
      countryName,
      description,
      createdDate,
      phoneNumber
    }
  }
`;

export const GET_FULL_USER_INFO = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      birthDate,
      displayName,
      email,
      firstname,
      lastname,
      createdDate,
      description,
      address,
      phoneNumber,
      genderLabel,
      countryName,
      statusLabel,
      userHashedId
    }
  }
`;

export const GET_USER_INFO_TO_UPDATE = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      birthDate,
      displayName,
      email,
      firstname,
      lastname,
      genderId,
      createdDate,
      description,
      address,
      countryId,
      phoneNumber,
      statusId,
      genderLabel,
      countryName
    }
  }
`;

export const UPDATE_USER_INFO_PER_ITEM = gpl`
  mutation UpdateUserInfoItem($criterias: UpdatePerItemInputType!){
    updateUserInfoItem(criterias: $criterias){
      isSuccess,
      errors {
        code
        description
      },
      result {
        value,
        propertyName
      }
    }
  }
`;
