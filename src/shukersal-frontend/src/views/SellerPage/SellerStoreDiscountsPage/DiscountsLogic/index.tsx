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
        alert(selected_id)
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
            display = <span>Additional Discount</span>;
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

const InputSelector = ({ option }: string) => {
    
};


const DiscountsLogic = ({ storeId }: DiscountsLogicProps) => {
    const [discounts, setDiscounts] = useState<DiscountRule[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [apiError, setApiError] = useState<string | null>(null);
    const [selectedNode, setSelectedNode] = useState<DiscountRule | null>(null);
    const [selectedId, setSelectedId] = useState('Click');
    const [value, setValue] = useState(1);
    const [value1, setValue1] = useState(1);
    const [value2, setValue2] = useState(1);

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


    let discount_options = <div><Listbox
        dataKey='id'
        textField='name'
        data={[
            { id: 0, name: "simple on store" },
            { id: 10, name: "simple on category" },
            { id: 20, name: "simple on product" },
            { id: 1, name: "conditional on store" },
            { id: 11, name: "conditional on category" },
            { id: 21, name: "conditional on product" },
            { id: 2, name: "xor max" },
            { id: 3, name: "xor min" },
            { id: 4, name: "xor conditional" },
            { id: 5, name: "Max discount" },
            { id: 6, name: "Additional discount discount" }]}
        value={value1}
        onChange={(nextValue) => setValue1(nextValue.id)}

    />
        {value1 >= 10 ? <div>product or category name<input></input></div> : null}
        {value1 % 10 <= 1 ? <div>discount amount<input></input></div> : null}
    </div>;

    let sub_discount_options = <div><Listbox
        dataKey='id'
        textField='name'
        data={[
            { id: 0, name: "and" },
            { id: 1, name: "or" },
            { id: 2, name: "condition" },
            { id: 3, name: "minimal product count" },
            { id: 4, name: "maximal product count" },
            { id: 5, name: "minimal category count" },
            { id: 6, name: "maximal category count" },
            { id: 7, name: "timed" }]}
        value={value2}
        onChange={(nextValue) => setValue2(nextValue.id)}

    />
        {value2 >= 3 && value2 <= 6 ? <div>product or category name<input></input>
            amount <input></input></div> :
            value2 == 7 ? <div>min hour<input></input>
                max hour <input></input></div>        :null}
        
    </div>;
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
                dataKey='id'
                textField='name'
                data={[
                    { id: 0, name: "new discount" },
                    { id: 1, name: "new purchase rule" },
                    { id: 2, name: "select discount" },
                    { id: 3, name: "select purchase rule" },
                    { id: 4, name: "add sub discount" },
                    { id: 5, name: "add condition" },
                    { id: 6, name: "add sub condition" },
                    { id: 7, name: "add sub purchase rule" }]}
                value={value}
                onChange={(nextValue) => setValue(nextValue.id)}

            />
            {value == 0 || value == 4?
                discount_options

                : value == 5 || value == 6 ?
                    sub_discount_options:
            null}
            <Typography variant="h5" >Selected Node ID: <strong>{value}</strong></Typography>
            <InputSelector option={value}></InputSelector>
            {apiError && <Typography variant="h6" color="error">{apiError}</Typography>}
        </>
    );
};

export default DiscountsLogic;