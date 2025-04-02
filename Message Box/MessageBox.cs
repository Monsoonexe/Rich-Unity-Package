using Cysharp.Threading.Tasks;
using RichPackage.UI.Transitions;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
 * On a message box that has an OK button, OK is returned if:
 *		a user clicks the OK button, 
 *		clicks the Close button in the title bar, or presses the ESC key.

	On a message box that has an OK button and a Cancel button, OK is returned if:
		a user clicks the OK button. 
		If a user clicks the Cancel button or the Close button in the title bar, Cancel is returned.

	On a message box that has a Yes button and a No button, Yes is returned if a user clicks the Yes button, and No is returned if a user clicks the No button.

	On a message box that has a Yes button, a No button, and a Cancel button, 
		Yes is returned if the Yes button is clicked and No is returned if the No button is clicked. 
		If a user clicks the Cancel button or the Close button in the title bar, Cancel is returned.
 * 
 */

//TODO raycast blocker

namespace RichPackage.UI
{
	public delegate void MessageBoxResultCallback(EMessageBoxResult result);

	/// <summary>
	/// Standard message box.
	/// </summary>
	[SelectionBase]
	public class MessageBox : RichUIElement, IMessageBox
	{
		#region Constants

		private const string YES = "Yes";
		private const string NO = "No";
		private const string OKAY = "Okay";
		private const string CANCEL = "Cancel";

		#endregion Constants

		#region Fields

		/// <summary>
		/// For loading really complex payloads.
		/// </summary>
		[Serializable]
		public struct Payload
		{
			[Serializable]
			public struct ButtonPayload
			{
				public UnityAction action;
				public string text;
				public Sprite sprite;
			}

			public string title;
			public string message;

			public Sprite boxImage;

			public ButtonPayload? button1Payload;

			public ButtonPayload? button2Payload;

			public ButtonPayload? button3Payload;
		}

		[Title("Prefab Refs")]
		[SerializeField, Required]
		private Canvas windowCanvas;

		[SerializeField, Required]
		private RectTransform windowTransform;

		[SerializeField, Required]
		private Image backgroundImage;

		[SerializeField, Required]
		private RichUIButton leftButton;

		[SerializeField, Required]
		private RichUIButton middleButton;

		[SerializeField, Required]
		private RichUIButton rightButton;

		[SerializeField, Required]
		private RichUIButton escapeButton;

		[SerializeField, Required]
		private TextMeshProUGUI promptText;

		[SerializeField, Required]
		private TextMeshProUGUI titleText;

		[Title("Settings")]
		public bool hideOnStart = true;

		[Title("Animation")]
		public bool animate = false;

		[SerializeField, ShowIf("@animate == true")]
		private ATransitionComponent transitionINAnimator;

		[SerializeField, ShowIf("@animate == true")]
		private ATransitionComponent transitionOUTAnimator;

		public event MessageBoxResultCallback OnResult;

		#endregion Fields

		public EMessageBoxButton Style { get; private set; } = EMessageBoxButton.OK;

		public EMessageBoxResult LastResult { get; private set; } = EMessageBoxResult.None;

		#region Unity Messages

		private void Start()
		{
			if (hideOnStart)
				Hide();
		}

		#endregion Unity Messages

		/// <param name="text">Main text body paragraph prompting user.</param>
		/// <param name="title">Title of the window.</param>
		/// <param name="style">Sets how many buttons and their default values.</param>
		/// <param name="resultCallback">Callback that will contain the choice determined by the User.</param>
		/// <param name="button1Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="button2Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="button3Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="animate">Should the window animate opened and closed?</param>
		[Button]
		public void Show(string text, string title = ConstStrings.EMPTY,
			EMessageBoxButton style = EMessageBoxButton.OK,
			bool animate = false,
			MessageBoxResultCallback resultCallback = null,
			string button1Text = null,
			string button2Text = null,
			string button3Text = null)
		{
			this.animate = animate;
			this.Style = style;
			promptText.text = text;
			titleText.text = title;

			OnResult = resultCallback;

			switch (style)
			{
				case EMessageBoxButton.OK:
					Debug.Assert(string.IsNullOrEmpty(button2Text), "Too many button texts supplied.");
					Debug.Assert(string.IsNullOrEmpty(button3Text), "Too many button texts supplied.");
					SetupSingleButton(button1Text);
					break;
				case EMessageBoxButton.OKCancel:
					Debug.Assert(string.IsNullOrEmpty(button3Text), "Too many button texts supplied.");
					SetupDoubleButton(button1Text, button2Text);
					break;
				case EMessageBoxButton.YesNo:
					Debug.Assert(string.IsNullOrEmpty(button3Text), "Too many button texts supplied.");
					SetupDoubleButton(button1Text, button2Text);
					break;
				case EMessageBoxButton.YesNoCancel:
					SetupTripleButton(button1Text, button2Text, button3Text);
					break;
				default:
					throw ExceptionUtilities.GetInvalidEnumCaseException(style);
			}

			Show();//show visuals
			if (animate && transitionINAnimator != null)
			{
				ToggleButtonInteractivity(false); //disallow interaction until open.
				transitionINAnimator.Animate(windowTransform,
					onCompleteCallback: () => ToggleButtonInteractivity(true));
			}
		}

		public override void ToggleVisuals()
			=> windowCanvas.enabled = !windowCanvas.enabled;

		public override void ToggleVisuals(bool active)
			=> windowCanvas.enabled = active;

		public void Close() => Close(EMessageBoxResult.None);

		public void Close(EMessageBoxResult result)
		{
			LastResult = result;

			ToggleButtonInteractivity(false); //disable, but don't hide.

			if (animate && transitionOUTAnimator != null)
				transitionOUTAnimator.Animate(windowTransform, Finally);
			else
				Finally();
		}

		private void Finally()
		{
			Hide(); //hide window
			if (OnResult != null)
			{
				//support chaining of windows, like "are you sure?". Prevent double-calling and lost callbacks
				MessageBoxResultCallback temp = OnResult; // save
				OnResult = null; // remove so next window could subscribe
				temp(LastResult); // can still call events without overlap
			}
		}

		private void SetButton(RichUIButton _button, in Payload.ButtonPayload? _payload)
		{
			if (_payload.HasValue)
			{
				UnityAction completeAction = _payload.Value.action; // do the thing
				completeAction += Close; // then close the window.
				_button.Show(completeAction, _payload.Value.text, _payload.Value.sprite);
			}
			else
			{
				_button.Hide();
			}
		}

		public void Show(in Payload payload)
		{
			titleText.text = payload.title;
			promptText.text = payload.message;
			backgroundImage.sprite = payload.boxImage;

			SetButton(leftButton, payload.button1Payload);
			SetButton(middleButton, payload.button2Payload);
			SetButton(rightButton, payload.button3Payload);
			escapeButton.Show(Cancel);

			Show();//show visuals
			if (animate && transitionINAnimator != null)
			{
				ToggleButtonInteractivityFalse(); // disallow interaction until open.
				transitionINAnimator.Animate(windowTransform,
					onCompleteCallback: ToggleButtonInteractivityTrue);
			}
		}

		#region Async

		/// <summary>
		/// Call like EMessageBoxResult result = await ShowAsync(...);
		/// </summary>
		/// <param name="messageBoxText">Main text body paragraph prompting user.</param>
		/// <param name="messageBoxTitle">Title of the window.</param>
		/// <param name="style">Sets how many buttons and their default values.</param>
		/// <param name="button1Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="button2Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="button3Text">Text on button. 'null' means use default value. 'Empty' means such.</param>
		/// <param name="animate">Should the window animate opened and closed?</param>
		public async UniTask<EMessageBoxResult> ShowAsync(string messageBoxText,
			string messageBoxTitle = ConstStrings.EMPTY,
			EMessageBoxButton style = EMessageBoxButton.OK,
			bool animate = false,
			string button1Text = null,
			string button2Text = null,
			string button3Text = null)
		{
			bool asyncResultPending = true;

			Show(messageBoxText, messageBoxTitle, style, animate,
				(_) => asyncResultPending = false, button1Text, button2Text, button3Text);

			while (asyncResultPending)
				await UniTask.Yield(); //basically yield return null;

			return LastResult;
		}

		#endregion Async

		/// <param name="buttonText">Will be "OK" if null.</param>
		private void SetupSingleButton(string buttonText = OKAY)
		{
			buttonText = buttonText ?? OKAY;

			leftButton.Hide();
			rightButton.Hide();

			middleButton.Show(Okay, buttonText);
			escapeButton.Show(Okay);
		}

		/// <param name="button1Text">Will be "Yes" if null.</param>
		/// <param name="button2Text">Will be "No" if null.</param>
		private void SetupDoubleButton(
			string button1Text = YES, string button2Text = NO)
		{
			button1Text = button1Text ?? YES;
			button2Text = button2Text ?? NO;

			middleButton.Hide();

			leftButton.Show(Yes, button1Text);
			rightButton.Show(No, button2Text);
			escapeButton.Show(No);
		}

		/// <param name="button1Text">Will be "Yes" if null.</param>
		/// <param name="button2Text">Will be "No" if null.</param>
		/// <param name="button3Text">Will be "Cancel" if null.</param>
		private void SetupTripleButton(
			string button1Text = YES, string button2Text = NO,
			string button3Text = CANCEL)
		{
			button1Text = button1Text ?? YES;
			button2Text = button2Text ?? NO;
			button3Text = button3Text ?? CANCEL;

			leftButton.Show(Yes, button1Text);
			middleButton.Show(No, button2Text);
			rightButton.Show(Cancel, button3Text);
			escapeButton.Show(Cancel, button3Text);
		}

		public void ResetWindow()
		{
			LastResult = EMessageBoxResult.None;
			OnResult = null;
		}

		private void ToggleButtonInteractivityTrue()
			=> ToggleButtonInteractivity(true);

		private void ToggleButtonInteractivityFalse()
			=> ToggleButtonInteractivity(false);

		private void ToggleButtonInteractivity(bool interactable)
		{
			leftButton.enabled = interactable;
			middleButton.enabled = interactable;
			rightButton.enabled = interactable;
			escapeButton.enabled = interactable;
		}

		#region Responses

		private void Okay() => Close(EMessageBoxResult.Okay);

		private void Yes() => Close(EMessageBoxResult.Yes);

		private void No() => Close(EMessageBoxResult.No);

		private void Cancel() => Close(EMessageBoxResult.Cancel);

		#endregion Responses
	}

	public enum EMessageBoxResult
	{
		/// <summary>
		/// Pending or cleared.
		/// </summary>
		None = 0,

		/// <summary>
		/// Acquiesce.
		/// Identical to <see cref="Left"/> and <see cref="Yes"/>.
		/// </summary>
		Okay = 1,

		/// <summary>
		/// Identical to <see cref="Left"/> and <see cref="Okay"/>.
		/// </summary>
		Yes = 1,

		/// <summary>
		/// Identical to <see cref="Yes"/> and <see cref="Okay"/>.
		/// </summary>
		Left = 1,

		/// <summary>
		/// Reject.
		/// Identical to <see cref="Middle"/>.
		/// </summary>
		No = 2,

		/// <summary>
		/// Identical to <see cref="No"/>.
		/// </summary>
		Middle = 2,

		/// <summary>
		/// Cancel
		/// Identical to <see cref="Right"/>.
		/// </summary>
		Cancel = 3,

		/// <summary>
		/// Identical to <see cref="Cancel"/>.
		/// </summary>
		Right = 3,
	}

	public enum EMessageBoxButton
	{
		/// <summary>
		/// Single button to ask the User to acknowledge something.
		/// </summary>
		OK = 0,

		/// <summary>
		/// 2 buttons to ask the User to accept or reject an operation.
		/// </summary>
		OKCancel = 1,

		/// <summary>
		/// 2 buttons. Virtually identical to OK/Cancel.
		/// </summary>
		YesNo = 2,

		/// <summary>
		/// The message box displays Yes, No, and Cancel buttons.
		/// </summary>
		YesNoCancel = 3,
	}
}
