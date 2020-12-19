import React, { Fragment, useEffect } from "react";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery, useMutation } from "@apollo/client";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import { GET_PRODUCT_FOR_UPDATE } from "../../utils/GraphQLQueries/queries";
import {
  VALIDATE_IMAGE_URL,
  FILTER_PRODUCT_CATEGORIES,
  UPDATE_PRODUCT,
  FILTER_FARMS,
} from "../../utils/GraphQLQueries/mutations";
import { withRouter } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import productCreationModel from "../../models/productCreationModel";
import ProductEditor from "../../components/organisms/ProfileEditors/ProductEditor";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const dispatch = useStore(false)[1];
  const [userFarms] = useMutation(FILTER_FARMS);
  const [productCategories] = useMutation(FILTER_PRODUCT_CATEGORIES);
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [updateProduct] = useMutation(UPDATE_PRODUCT, {
    client: graphqlClient,
  });

  const { loading, data, error, refetch, called } = useQuery(
    GET_PRODUCT_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const onImageValidate = async (value) => {
    return await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });
  };

  // Selections mapping
  const mapSelectListItems = (response) => {
    var { data } = response;
    var { categories } = data;
    if (!categories) {
      return [];
    }
    return categories.map((cat) => {
      return {
        value: cat.id,
        label: cat.text,
      };
    });
  };

  const searchProductCategories = async (inputValue) => {
    return await productCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        return mapSelectListItems(response);
      })
      .catch((error) => {
        return [];
      });
  };

  const searchFarms = async (inputValue) => {
    return await userFarms({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { userFarms } = data;
        if (!userFarms) {
          return [];
        }
        return userFarms.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        return [];
      });
  };

  const onProductPost = async (data) => {
    return await updateProduct({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { updateProduct: product } = data;
        if (props.location.state && props.location.state.from) {
          const referrefUri = props.location.state.from;
          const productUpdateUrl = `/products/update/${product.id}`;
          if (referrefUri !== productUpdateUrl) {
            raiseProductUpdatedNotify(product);
            props.history.push(referrefUri);
            resolve({ product });
            return;
          }
        }

        raiseProductUpdatedNotify(product);
        props.history.push(`/products/${product.id}`);
        resolve({ product });
      });
    });
  };

  const raiseProductUpdatedNotify = (product) => {
    dispatch("PRODUCT_UPDATE", {
      id: product.id,
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { product } = data;

  const currentProduct = JSON.parse(JSON.stringify(productCreationModel));
  for (const formIdentifier in currentProduct) {
    currentProduct[formIdentifier].value = product[formIdentifier];
    if (product[formIdentifier]) {
      currentProduct[formIdentifier].isValid = true;
    }
  }

  const breadcrumbs = [
    {
      title: "Products",
      url: "/products/",
    },
    {
      url: `/products/${product.id}`,
      title: product.name,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <ProductEditor
        currentProduct={currentProduct}
        height={350}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchProductCategories}
        onProductPost={onProductPost}
        showValidationError={showValidationError}
        filterFarms={searchFarms}
      />
    </Fragment>
  );
});
