namespace com.eze.api {
public class TransactionDetails {

    private string pmtType;
	private string status;
	private string txnId;
	private double amount;
	private string settlementStatus;
	private bool voidable;
	private string authCode;
	private string cardType;
	private string orderId;
	private string tid;
	

	public string getPmtType() {
		return pmtType;
	}
	public void setPmtType(string pmtType) {
		this.pmtType = pmtType;
	}
	public string getStatus() {
		return status;
	}
	public void setStatus(string status) {
		this.status = status;
	}
	public string getTxnId() {
		return txnId;
	}
	public void setTxnId(string txnId) {
		this.txnId = txnId;
	}
	public double getAmount() {
		return amount;
	}
	public void setAmount(double amount) {
		this.amount = amount;
	}
	public string getSettlementStatus() {
		return settlementStatus;
	}
	public void setSettlementStatus(string settlementStatus) {
		this.settlementStatus = settlementStatus;
	}
	public bool getVoidable() {
		return voidable;
	}
	public void setVoidable(bool voidable) {
		this.voidable = voidable;
	}
	
	public string getAuthCode() {
		return authCode;
	}
	public void setAuthCode(string authCode) {
		this.authCode = authCode;
	}
	public string getCardType() {
		return cardType;
	}
	public void setCardType(string cardType) {
		this.cardType = cardType;
	}
	public string getOrderId() {
		return orderId;
	}
	public void setOrderId(string orderId) {
		this.orderId = orderId;
	}
	public string getTid() {
		return tid;
	}
	public void setTid(string tid) {
		this.tid = tid;
	}
	
	public override string ToString() {
		return "PaymentResult [pmtType=" + pmtType + ", status=" + status + ", txnId=" + txnId + ", amount=" + amount
				+ ", settlementStatus=" + settlementStatus + ", voidable=" + voidable +  ", authCode=" + authCode + ", cardType=" + cardType + ", orderId="
				+ orderId + ", tid=" + tid+"]";
	}
}
}