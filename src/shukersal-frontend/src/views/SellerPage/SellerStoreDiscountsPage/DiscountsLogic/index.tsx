//@ts-nocheck
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

export interface PurchaseRule { }

interface DiscountsLogicProps {
    storeId: number;
}

let selected_id = -1

//let selected_type = 0

const TreeNode = ({ node }) => {
    const { id, discountType, discount, components, discountOn, discountOnString, discountRuleBoolean } = node;
    let display;
    let discountdisplay;
    let backgroundcolor = '#FFFFFF'
    const [selected, setSelected] = useState(false);
    const handleClick = () => {

        selected_id = id
        alert("node selected");
        backgroundcolor = '#AACCFF'

    };

    switch (discountOn) {
        case 0:
            discountdisplay = <span>{discount}% discount</span>
            break;
        case 1:
            discountdisplay = <span>{discount}% discount on {discountOnString} category:</span>
            break;
        case 2:
            discountdisplay = <span>{discount}% discount on {discountOnString} product:</span>
            break;
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
                <tr><td><ul>{discountRuleBoolean ? <TreeNodeB node={discountRuleBoolean} /> : null}</ul></td></tr></table >
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
    const handleClick = () => {

        selected_id = id
        alert("node selected");
        backgroundcolor = '#AACCFF'

    };
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
            <button onClick={handleClick}>Select</button>
            {display}
            <ul>
                {components.map((component) => (
                    <li key={component.id}>
                        <TreeNodeB node={component} onSelect={onSelect} />
                    </li>
                ))}
            </ul>
        </div>
    );
    
};

const TreeNodeC = ({ node }) => {
    const { id, purchaseRuleType, components, conditionString, conditionLimit, minHour, maxHour } = node;
    let display;
    const handleClick = () => {

        selected_id = id
        alert("node selected");
        backgroundcolor = '#AACCFF'

    };
    switch (purchaseRuleType) {
        case 0:
            // Render display for discountType 0
            display = <span>And Purchase Rule</span>
            break;
        case 1:
            // Render display for discountType 1
            display = <span>Or Purchase Rule</span>
            break;
        case 2:
            // Render display for discountType 5
            display = <span>Conditional Purchase Rule</span>;
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
            <button onClick={handleClick}>Select</button>
            {display}
            <ul>
                {components.map((component) => (
                    <li key={component.id}>
                        <TreeNodeC node={component} onSelect={onSelect} />
                    </li>
                ))}
            </ul>
        </div>
    );

};


const InputSelector = ({ option }: string) => {
    return <></>
};


const DiscountsLogic = ({ storeId }: DiscountsLogicProps) => {
    const [discounts, setDiscounts] = useState<DiscountRule[]>([]);
    const [sdiscount, setsDiscount] = useState<DiscountRule[]>([]);
    const [purchaserules, setPurchaserules] = useState<PurchaseRule[]>([]);
    const [spurchaserule, setsPurchaserule] = useState<PurchaseRule[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [apiError, setApiError] = useState<string | null>(null);
    const [selectedNode, setSelectedNode] = useState<DiscountRule | null>(null);
    const [selectedId, setSelectedId] = useState('Click');
    const [value, setValue] = useState(0);
    const [value1, setValue1] = useState(0);
    const [value2, setValue2] = useState(0);
    const [value3, setValue3] = useState(0);
    const [value4, setValue4] = useState(0);
    const [discount, setDiscount] = useState('10')
    const [discountOn, setdiscountOn] = useState('')
    const [discountOnCheck, setdiscountOnCheck] = useState('')
    const [discountLimit, setdiscountLimit] = useState('1')
    const [minhour, setminhour] = useState('0')
    const [maxhour, setmaxhour] = useState('24')

    const loaddata = () => {
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
        discountsApi
            .getSlectedDiscount(storeId)
            .then((res) => {
                const responseDiscounts = res as ApiListData<DiscountRule>;
                setIsLoading(false);
                setApiError(null);
                setsDiscount([responseDiscounts]);
            })
            .catch((res) => {
                const error = res as ApiError;
                setIsLoading(false);
                setApiError(error.message ?? "Error.");
                setsDiscount(null);
            });
        discountsApi
            .getAllPR(storeId)
            .then((res) => {
                const responseDiscounts = res as ApiListData<PurchaseRule>;
                setIsLoading(false);
                setApiError(null);
                setPurchaserules(responseDiscounts);
            })
            .catch((res) => {
                const error = res as ApiError;
                setIsLoading(false);
                setApiError(error.message ?? "Error.");
                setPurchaserules([]);
            });
        discountsApi
            .getSelectedPR(storeId)
            .then((res) => {
                const responseDiscounts = res as ApiListData<PurchaseRule>;
                setIsLoading(false);
                setApiError(null);
                setsPurchaserule([responseDiscounts]);
            })
            .catch((res) => {
                const error = res as ApiError;
                setIsLoading(false);
                setApiError(error.message ?? "Error.");
                setsPurchaserule(null);
            });

    };

    useEffect(() => loaddata()

    , []);

    const handleNodeSelect = (node: DiscountRule) => {
        setSelectedNode(node);
    };

    if (isLoading)
        return <AppLoader />;


    const discount_options = <div><Listbox
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
        {value1 >= 10 ? <div>product or category name<input
            value={discountOn}
            onChange={e => setdiscountOn(e.target.value)}
        ></input></div> : null}
        {value1 % 10 <= 1 ? <div>discount amount<input
            value={discount}
            onChange={e => setDiscount(e.target.value)}
            type="number"        ></input></div> : null}
    </div>;

    const sub_discount_options = <div><Listbox
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
        {value2 >= 3 && value2 <= 6 ? <div>product or category name<input
            value={discountOnCheck}
            onChange={e => setdiscountOnCheck(e.target.value)}
        ></input>
            amount <input
                value={discountLimit}
                onChange={e => setdiscountLimit(e.target.value)}
                type="number"
            ></input></div> :
            value2 == 7 ? <div>min hour<input
                value={minhour}
                onChange={e => setminhour(e.target.value)}
                type="number"
            ></input>
                max hour <input
                    value={maxhour}
                    onChange={e => setmaxhour(e.target.value)}
                    type="number"
                ></input></div> : null}
        
    </div>;

    const pr_options = <div><Listbox
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
        value={value3}
        onChange={(nextValue) => setValue3(nextValue.id)}

    />
        {value3 >= 3 && value3 <= 6 ? <div>product or category name<input
            value={discountOnCheck}
            onChange={e => setdiscountOnCheck(e.target.value)}
        ></input>
            amount <input
                value={discountLimit}
                onChange={e => setdiscountLimit(e.target.value)}
                type="number"
            ></input></div> :
            value3 == 7 ? <div>min hour<input
                value={minhour}
                onChange={e => setminhour(e.target.value)}
                type="number"
            ></input>
                max hour <input
                    value={maxhour}
                    onChange={e => setmaxhour(e.target.value)}
                    type="number"
                ></input></div> : null}

    </div>;
    return (
        <>
            <Typography variant="h3">Discounts Logic for store {storeId}!</Typography>
            <Listbox
                dataKey='id'
                textField='name'
                data={[
                    { id: 0, name: "all discounts" },
                    { id: 1, name: "selected discount" },
                    { id: 2, name: "all purchase rules" },
                    { id: 3, name: "selected purchase rule" }]}
                value={value4}
                onChange={(nextValue) => setValue4(nextValue.id)}

            />
            <Typography variant="h4">Here are your discounts:</Typography>
            {value4 == 0? discounts.map((discount) => (
                <div key={discount.id}>
                    <TreeNode node={discount} />
                </div> 
            )) :
            value4 == 1 ? sdiscount.map((discount) => (
                <div key={discount.id}>
                    {discount ? <TreeNode node={discount} /> : null}
                </div>
                )) :
            value4 == 2 ? purchaserules.map((pr) => (
                <div key={pr.id}>
                    <TreeNodeC node={pr} />
                </div>
            )) :
            value4 == 3 ? spurchaserule.map((pr) => (
                <div key={pr.id}>
                    {pr ? <TreeNodeC node={pr} /> : error}
                </div>
        )): null}
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
                discount_options:
             value == 5 || value == 6 ?
                sub_discount_options :
             value == 1 || value == 7 ?
                pr_options:
            null}
            {
                value == 0 ? <button onClick={() => { discountsApi.createNewDiscount(storeId, value1, discount, discountOn); window.location.reload(); }}>Add</button> :
                value == 1 ? <button onClick={() => { discountsApi.createPR(storeId, value3, discountLimit, discountOnCheck, minhour, maxhour); window.location.reload(); }}>Add</button> :
                value == 2 ? <button onClick={() => { discountsApi.selectDiscount(storeId, selected_id); window.location.reload();}} >Select</button  > :
                value == 3 ? <button onClick={() => { discountsApi.selectPRule(storeId, selected_id); window.location.reload();}}>Select</button> :
                value == 4 ? <button onClick={() => { discountsApi.createChildDiscount(storeId, value1, discount, discountOn, selected_id); window.location.reload();}}>Add</button> :
                value == 5 ? <button onClick={() => { discountsApi.createConditionalDiscount(storeId, value2, discountLimit, discountOnCheck, minhour, maxhour, selected_id); window.location.reload();}}>Add</button> :
                value == 6 ? <button onClick={() => { discountsApi.createConditionalChildDiscount(storeId, value2, discountLimit, discountOnCheck, minhour, maxhour, selected_id); window.location.reload();}}>Add</button> :
                value == 7 ? <button onClick={() => { discountsApi.createChildPR(storeId, value3, discountLimit, discountOnCheck, minhour, maxhour, selected_id); window.location.reload(); }}>Add</button> :
            null}
            <InputSelector option={value}></InputSelector>
            {apiError && <Typography variant="h6" color="error">{apiError}</Typography>}
        </>
    );
};

export default DiscountsLogic;