using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Script : MonoBehaviour
{
	[SerializeField] private CanvasGroup StartMenu_CG;
	[SerializeField] private CanvasGroup YNTarot_HowTo_CG;
	[SerializeField] private CanvasGroup YNTarot_MainFlow_CG;

	private Tween fadeTween;

	// Start is called before the first frame update
	void Start()
	{
		FadeInStartMenu(2f);
	}

	// --- Main function used in all fades ---
	private void Fade(float endValue, float duration, TweenCallback onEnd)
	{
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}

		fadeTween = StartMenu_CG.DOFade(endValue, duration);
		fadeTween.onComplete += onEnd;
	}

	private void Fade_YN_HowTo(float endValue, float duration, TweenCallback onEnd)
	{
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}

		fadeTween = YNTarot_HowTo_CG.DOFade(endValue, duration);
		fadeTween.onComplete += onEnd;
	}

	private void Fade_YN_MainFlow(float endValue, float duration, TweenCallback onEnd)
	{
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}

		fadeTween = YNTarot_MainFlow_CG.DOFade(endValue, duration);
		fadeTween.onComplete += onEnd;
	}


	// ----------- Main Menu Fadings -------------
	public void FadeInStartMenu(float duration)
	{
		Fade(1f, duration, () =>
		{
			StartMenu_CG.interactable = true;
			StartMenu_CG.blocksRaycasts = true;
		});
	}

	public void FadeOutStartMenu(float duration)
	{
		Fade(0f, duration, () =>
		{
			StartMenu_CG.interactable = false;
			StartMenu_CG.blocksRaycasts = false;
		});
	}


	// ---------------- YES NO Tarot Fadings -------------
	// --- Yes/No: How to Panel ---
	public void FadeIn_YN_HowTo(float duration)
	{
		Fade_YN_HowTo(1f, duration, () =>
		{
			YNTarot_HowTo_CG.interactable = true;
			YNTarot_HowTo_CG.blocksRaycasts = true;
		});
	}

	public void FadeOut_YN_HowTo(float duration)
	{
		Fade_YN_HowTo(0f, duration, () =>
		{
			YNTarot_HowTo_CG.interactable = false;
			YNTarot_HowTo_CG.blocksRaycasts = false;
		});
	}

	// --- Yes/No: Main Flow Panel ---
	public void FadeIn_YN_MainFlow(float duration)
	{
		Fade_YN_MainFlow(1f, duration, () =>
		{
			YNTarot_MainFlow_CG.interactable = true;
			YNTarot_MainFlow_CG.blocksRaycasts = true;
		});
	}

	public void FadeOut_YN_MainFlow(float duration)
	{
		Fade_YN_MainFlow(0f, duration, () =>
		{
			YNTarot_MainFlow_CG.interactable = false;
			YNTarot_MainFlow_CG.blocksRaycasts = false;
		});
	}



}