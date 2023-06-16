import { useEffect, useState } from "react";
import { discountsApi } from "./discountsApi";
import { ApiError, ApiListData } from "../../../../types/apiTypes";
import { Typography } from '@mui/material';
import { AppLoader } from "../../../../components/AppLoader/AppLoader";
import "react-widgets/styles.css";
import Listbox from "react-widgets/Listbox";


interface DiscountRule {
    id : number
    // Define your DiscountRule interface here
}

interface DiscountsLogicProps {
    storeId: number;
}

var selected_id = -1

var selected_type = 0

const TreeNode = ({ node }) => {
    const { id, discountType, discount, components, discountOn, discountOnString, discountRuleBoolean } = node;
    let display;
    let discountdisplay;
    var backgroundcolor = '#FFFFFF'
    const [selected, setSelected] = useState(false);
    const handleClick = () => {

        selected_id = id
        var backgroundcolor = '#AACCFF'

    };

    switch (discountOn) {
        case 0:
            discountdisplay = <span>{discount}% discount</span>
        case 1:
            discountdisplay = <span>{discount}% discount on {discountOnString} category:</span>
        case 2:
            discountdisplay = <span>{discount}% discount on {discountOnString} product:</span>
        default:
            discountdisplay = <span>{discount}% discount</span>
    }


    switch (discountType) {
        case 0:
            // Render display for discountType 0
            display =<table><tr><td>
                Simple Discount</td></tr>
                <tr><td>{discountdisplay}</td></tr></table>
            ;
            break;
        case 1:
            // Render display for discountType 1
            display = <table><tr><td>
                Conditional Discount</td></tr>
                <tr><td>{discountdisplay}</td></tr>
                <tr><td><ul><TreeNodeB node={discountRuleBoolean} /></ul></td></tr></table >
            break;
        case 2:
            // Render display for discountType 5
            display = <span>Max Xor Discount</span>;
            break;
        case 3:
            // Render display for discountType 5
            display = <span>Mix Xor Discount</span>;
            break;
        case 4:
            // Render display for discountType 5
            display = <span>Conditional Xor Discount</span>;
            break;
        case 5:
            // Render display for discountType 5
            display = <span>Max Discount</span>;
            break;
        case 6:
            // Render display for discountType 5
            display = <span>Min Discount</span>;
            break;
        default:
            // Render a default display for other discountType values
            display = <span>Unknown Discount Type</span>;
            break;
    }

    return (
        
        <div>
            <button onClick={handleClick}>Select</button>
            <div style={{ backgroundColor: backgroundcolor }} >{display}</div>
            <ul>
                {components.map((component) => (
                    
                        <TreeNode node={component} />
                    
                ))}
            </ul>
        </div>
    );
};

const TreeNodeB = ({ node }) => {
    const { id, discountRuleBooleanType, components, conditionString, conditionLimit, minHour, maxHour } = node;
    let display;
    switch (discountRuleBooleanType) {
        case 0:
            // Render display for discountType 0
            display = <span>And Discount</span>
            break;
        case 1:
            // Render display for discountType 1
            display = <span>Or Discount</span>
            break;
        case 2:
            // Render display for discountType 5
            display = <span>Conditional Discount</span>;
            break;
        case 3:
            // Render display for discountType 5
            display = <span>minimal {conditionString} product count : {conditionLimit}</span>;
            break;
        case 4:
            // Render display for discountType 5
            display = <span>maximal {conditionString} product count : {conditionLimit}</span>;
            break;
        case 5:
            // Render display for discountType 5
            display = <span>minimal {conditionString} category count : {conditionLimit}</span>;
            break;
        case 6:
            // Render display for discountType 5
            display = <span>maximal {conditionString} category count : {conditionLimit}</span>;
            break;
        case 7:
            display = <span>applies from {minHour}:00 to {maxHour}:00</span>;
            break;
        default:
            // Render a default display for other discountType values
            display = <span>please insert a discount condition</span>;
            break;
    }
    return (
        <div>
            {display}
            <ul>
                {components.map((component) => (
                    <li key={component.id}>
                        <TreeNode node={component} onSelect={onSelect} />
                    </li>
                ))}
            </ul>
        </div>
    );
    
};

const InputSelector = ({ option } : string) => {
    if ({ option } === "new discount")
        return (<div>a + {option}</div>);
    else
        return (<div>{option}</div>);
};


const DiscountsLogic = ({ storeId }: DiscountsLogicProps) => {
    const [discounts, setDiscounts] = useState<DiscountRule[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [apiError, setApiError] = useState<string | null>(null);
    const [selectedNode, setSelectedNode] = useState<DiscountRule | null>(null);
    const [selectedId, setSelectedId] = useState('Click');
    const [value, setValue] = useState(1);

    useEffect(() => {
        setIsLoading(true);
        discountsApi
            .getAll(storeId)
            .then((res) => {
                const responseDiscounts = res as ApiListData<DiscountRule>;
                setIsLoading(false);
                setApiError(null);
                setDiscounts(responseDiscounts);
            })
            .catch((res) => {
                const error = res as ApiError;
                setIsLoading(false);
                setApiError(error.message ?? "Error.");
                setDiscounts([]);
            });
    }, []);

    const handleNodeSelect = (node: DiscountRule) => {
        setSelectedNode(node);
    };

    if (isLoading)
        return <AppLoader />;

    return (
        <>
            <Typography variant="h3">Discounts Logic for store {storeId}!</Typography>
            <Typography variant="h4">Here are your discounts:</Typography>
            {discounts.map((discount) => (
                <div key={discount.id}>
                    <TreeNode node={discount} />
                </div>
            ))}
            <Listbox
                data={["new discount", "new purchase rule", "select discount", "select purchase rule", "add sub discount", "add condition", "add sub condition", "add sub purchase rule"]}
                value={value}
                onChange={(nextValue) => setValue(nextValue)}

            />;
            <Typography variant="h5" >Selected Node ID: <strong>{value}</strong></Typography>
            <InputSelector option={value}></InputSelector>
            {apiError && <Typography variant="h6" color="error">{apiError}</Typography>}
        </>
    );
};

export default DiscountsLogic;