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
  mutation signin($signinModel: SigninInputType!){
    signin(signinModel: $signinModel){
      result {
        userInfo {
          displayName,
          userIdentityId
        },
        authenticationToken,
      },
      isSuccess,
      errors {
        code
        description
      }
    }
  }
`;

export const UPDATE_USER_PROFILE = gpl`
  mutation UpdateUserProfile($user: RegisterInputType!){
    updateUserProfile(user: $user){
      isSuccess,
      errors {
        code,
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
      userIdentityId,
      avatarUrl,
      coverPhotoUrl
    }
  }
`;

export const GET_USER_INFO = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      accessMode
      result{
        email
        displayName
        userIdentityId
        address
        birthDate
        countryName
        description
        createdDate
        phoneNumber,
        avatarUrl,
        coverPhotoUrl
      }
    }
  }
`;

export const GET_FULL_USER_INFO = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      accessMode
      result {
        birthDate
        displayName
        email
        firstname
        lastname
        createdDate
        description
        address
        phoneNumber
        genderLabel
        genderId
        countryName
        countryId
        statusLabel
        userIdentityId
        coverPhotoUrl
        genderSelections {
          id
          text
        }
        countrySelections {
          id
          text
        }
      }
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

export const UPDATE_USER_AVATAR = gpl`
  mutation UpdateAvatar($criterias: UpdateUserPhotoInputType!){
    updateAvatar(criterias: $criterias){
      isSuccess,
      errors {
        code
        description
      },
      result {
        photoUrl,
        xAxis,
        yAxis,
        width,
        height,
        contentType,
        canEdit
      }
    }
  }
`;

export const DELETE_USER_AVATAR = gpl`
  mutation DeleteAvatar($criterias: DeleteUserPhotoInputType!){
    deleteAvatar(criterias: $criterias){
      isSuccess,
      errors {
        code
        description
      },
      result {
        canEdit
      }
    }
  }
`;

export const UPDATE_USER_COVER = gpl`
  mutation UpdateUserCover($criterias: UpdateUserPhotoInputType!){
    updateUserCover(criterias: $criterias){
      isSuccess,
      errors {
        code
        description
      },
      result {
        photoUrl,
        xAxis,
        yAxis,
        width,
        height,
        contentType,
        canEdit
      }
    }
  }
`;

export const DELETE_USER_COVER = gpl`
  mutation DeleteCover($criterias: DeleteUserPhotoInputType!){
    deleteCover(criterias: $criterias){
      isSuccess,
      errors {
        code
        description
      },
      result {
        canEdit
      }
    }
  }
`;
